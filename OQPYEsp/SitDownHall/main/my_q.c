#include "malloc.h"
#include <freertos/heap_regions.h>

#include "esp_heap_alloc_caps.h"
#include "esp_log.h"

#include "my_q.h"
const static char* TAG = "my_q";

void push_my_q(my_q_t* head, my_data_t data)
{
	my_list_t* new_data = pvPortMallocCaps(sizeof(my_list_t), MALLOC_CAP_32BIT);
	new_data->next = NULL;
	new_data->data = data;
	if (head->first == NULL) {
		head->first = new_data;
	}
	if (head->last != NULL)
		head->last->next = new_data;
	head->last = new_data;
	head->count = head->count + 1;
	ESP_LOGI(TAG, "Qcout: %d", head->count);
	return;
}
my_data_t pop_my_q(my_q_t* head) {
	my_data_t ret = {
			.on_off = 0,
			.hall_data = 0,
	};
	my_list_t* my_first;
	ESP_LOGI(TAG, "POP first: %p  last: %p  count: %d", head->first, head->last, head->count);
	if (head->first == NULL) {
		return ret;
	}
	ret = head->first->data;
	my_first = head->first;
	head->first = head->first->next;
	free(my_first);
	head->count = (head->count) - 1;
	ESP_LOGI(TAG, "POP Qcout: %d", head->count);
	return ret;
}

my_q_t *init_my_q() {
	my_q_t* ret_q = pvPortMallocCaps(sizeof(my_q_t), MALLOC_CAP_32BIT);
	ret_q->count = 0;
	ret_q->last = NULL;
	ret_q->first = NULL;
	return ret_q;
}