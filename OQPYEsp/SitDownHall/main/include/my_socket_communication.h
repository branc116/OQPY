#define WIFI_SSID CONFIG_WIFI_SSID
#define WIFI_PASS CONFIG_WIFI_PASSWORD
#define ESP_ID CONFIG_ESP_ID
#define HOST CONFIG_HOST
#define PORT CONFIG_PORT
#define ROUTE CONFIG_ROUTE

#define WEB_SERVER HOST
#define WEB_PORT PORT

esp_err_t event_handler(void *ctx, system_event_t *event);

void http_post_task(void* headp);

void initialise_wifi(void);