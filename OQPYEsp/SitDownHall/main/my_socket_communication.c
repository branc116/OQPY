#include <string.h>
#include "esp_wifi.h"
#include "esp_event_loop.h"
#include "esp_log.h"
#include "lwip/err.h"
#include "lwip/sockets.h"
#include "lwip/sys.h"
#include "lwip/netdb.h"
#include "lwip/dns.h"
#include "freertos/event_groups.h"
#include "driver/adc.h"

#include "my_q.h"
#include "my_socket_communication.h"

static char *POST = "GET "ROUTE" HTTP/1.1\r\n"
"Host: "WEB_SERVER":"WEB_PORT"\r\n"
"User-Agent: esp-idf/1.0 esp32\r\n"
"Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8\r\n"
"Accept-Language: en-US,en;q=0.5\r\n"
"Accept-Encoding: gzip, deflate\r\n"
"Connection: keep-alive\r\n"
"Upgrade-Insecure-Requests: 1\r\n"
"Cache-Control: max-age=0\r\n"
"espId: "ESP_ID"\r\n";
static char *TAG = "sockets";

const int CONNECTED_BIT = BIT0;

static EventGroupHandle_t wifi_event_group;

esp_err_t event_handler(void *ctx, system_event_t *event)
{
	switch (event->event_id) {
	case SYSTEM_EVENT_STA_START:
		esp_wifi_connect();
		ESP_LOGI(TAG, "Started, connecting");
		break;
	case SYSTEM_EVENT_STA_GOT_IP:
		xEventGroupSetBits(wifi_event_group, CONNECTED_BIT);
		ESP_LOGI(TAG, "Connected");
		break;
	case SYSTEM_EVENT_STA_DISCONNECTED:
		/* This is a workaround as ESP32 WiFi libs don't currently
		   auto-reassociate. */
		esp_wifi_connect();
		xEventGroupClearBits(wifi_event_group, CONNECTED_BIT);
		ESP_LOGI(TAG, "Disconnected ... Trying to reconnect");
		break;
	default:
		break;
	}
	return ESP_OK;
}

void initialise_wifi(void)
{
	tcpip_adapter_init();
	wifi_event_group = xEventGroupCreate();
	ESP_ERROR_CHECK(esp_event_loop_init(event_handler, NULL));
	wifi_init_config_t cfg = WIFI_INIT_CONFIG_DEFAULT();
	ESP_ERROR_CHECK(esp_wifi_init(&cfg));
	ESP_ERROR_CHECK(esp_wifi_set_storage(WIFI_STORAGE_RAM));
	wifi_config_t wifi_config = {
		.sta = {
			.ssid = WIFI_SSID,
			.password = WIFI_PASS,
		},
	};
	ESP_LOGI(TAG, "Setting WiFi configuration SSID %s...", wifi_config.sta.ssid);
	ESP_ERROR_CHECK(esp_wifi_set_mode(WIFI_MODE_STA));
	ESP_ERROR_CHECK(esp_wifi_set_config(ESP_IF_WIFI_STA, &wifi_config));
	ESP_ERROR_CHECK(esp_wifi_start());
	ESP_ERROR_CHECK(adc1_config_width(ADC_WIDTH_12Bit));
}

void manage_sending(my_data_t data)
{
	const struct addrinfo hints = {
		.ai_family = AF_INET,
		.ai_socktype = SOCK_STREAM,
	};
	struct addrinfo *res;
	char req[1024], ress[512];
	int s, my_ok;
	my_ok = false;

	while (!my_ok)
	{
		xEventGroupWaitBits(wifi_event_group, CONNECTED_BIT,
			false, true, portMAX_DELAY);
		int err = getaddrinfo(WEB_SERVER, WEB_PORT, &hints, &res);
		if (err != 0 || res == NULL) {
			ESP_LOGE(TAG, "DNS lookup failed err=%d res=%p", err, res);
			vTaskDelay(4000 / portTICK_PERIOD_MS);
			continue;
		}
		s = socket(res->ai_family, res->ai_socktype, 0);
		if (s < 0) {
			ESP_LOGE(TAG, "... Failed to allocate socket.");
			freeaddrinfo(res);
			vTaskDelay(4000 / portTICK_PERIOD_MS);
			continue;
		}

		if (connect(s, res->ai_addr, res->ai_addrlen) != 0) {
			ESP_LOGE(TAG, "... socket connect failed errno=%d", errno);
			close(s);
			freeaddrinfo(res);
			vTaskDelay(4000 / portTICK_PERIOD_MS);
			continue;
		}

		bzero(req, sizeof(req));
		freeaddrinfo(res);
		sprintf(req, "%srawData: %u\r\nonOff: %d\r\n\r\n", POST, data.hall_data, data.on_off);
		ESP_LOGI(TAG, "Request:\r\n%s", req);
		if (write(s, req, strlen(req)) < 0) {
			ESP_LOGE(TAG, "... socket send failed");
			close(s);
			vTaskDelay(4000 / portTICK_PERIOD_MS);
			continue;
		}
		bzero(ress, sizeof(ress));
		recv(s, ress, sizeof(ress), 0);
		ESP_LOGI(TAG, "Response:\r\n%s", ress);
		close(s);
		my_ok = true;
	}
}
void http_post_task(void* headp)
{
	my_data_t my_hal_sensor_curent_value;
	my_q_t* head = ((my_q_t*)headp);
	// ESP_LOGI(TAG, "DNS lookup succeeded. IP=%s", inet_ntoa(*addr));
	while (1) {
		if (head->count > 0)
		{
			ESP_LOGI(TAG, "POP Qcout: %d", head->count);
			my_hal_sensor_curent_value = pop_my_q(head);
			manage_sending(my_hal_sensor_curent_value);
		}
		else {
			vTaskDelay(1000 / portTICK_PERIOD_MS);
		}
	}
}