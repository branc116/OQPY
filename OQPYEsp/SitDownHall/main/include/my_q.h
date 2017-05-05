typedef struct my_data {
	uint16_t on_off;
	uint16_t hall_data;
}my_data_t;

typedef struct my_list {
	/* On = 1, Off = 0 */
	my_data_t data;
	struct my_list* next;
}my_list_t;
typedef struct my_q {
	my_list_t* first;
	my_list_t* last;
	int count;
}my_q_t;

void push_my_q(my_q_t* head, my_data_t data);

my_data_t pop_my_q(my_q_t* head);

my_q_t *init_my_q();