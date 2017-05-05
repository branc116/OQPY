/* HTTP GET Example using plain POSIX sockets

   This example code is in the Public Domain (or CC0 licensed, at your option.)

   Unless required by applicable law or agreed to in writing, this
   software is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
   CONDITIONS OF ANY KIND, either express or implied.
*/
#include "freertos/FreeRTOS.h"
#include "esp_event_loop.h"
#include "esp_log.h"
#include "nvs_flash.h"
#include "my_q.h"
#include "my_socket_communication.h"
#include "my_hall_handling.h"

my_q_t* my_global_q_head;

void app_main()
{
	ESP_ERROR_CHECK(nvs_flash_init());
	initialise_wifi();
	my_global_q_head = init_my_q();

	xTaskCreate(&http_post_task, "http_post_task", 4096, my_global_q_head, 5, NULL);
	read_hall_task(my_global_q_head);
}