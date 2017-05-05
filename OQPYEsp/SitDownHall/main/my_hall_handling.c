#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "driver/adc.h"
#include "esp_log.h"
#include "my_q.h"
#include "my_hall_handling.h"

static const char *TAG = "hall";

void read_hall_task(my_q_t* head)
{
	my_data_t data = {
		.hall_data = 130,
		.on_off = 0,
	};
	uint16_t new_hall = hall_sensor_read();
	int new_state;
	push_my_q(head, data);
	while (1)
	{
		new_hall = hall_sensor_read();
		new_state = ON_CHAIR_OF_CHAIR(new_hall);
		// ESP_LOGI(TAG, "Hall sensor read = %u, Hall state = %u", my_hall, new_state);
		if (new_state != data.on_off)
		{
			vTaskDelay(((REFRESH_TIME) * 5) / portTICK_PERIOD_MS);
			new_hall = hall_sensor_read();
			new_state = ON_CHAIR_OF_CHAIR(new_hall);
			if (new_state != data.on_off) {
				ESP_LOGI(TAG, "Hall sensor read = %u, Hall state = %u", new_hall, new_state);
				data.on_off = new_state;
				data.hall_data = new_hall;
				push_my_q(head, data);
			}
		}
		vTaskDelay((REFRESH_TIME) / portTICK_PERIOD_MS);
	}
}