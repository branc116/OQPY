#define MIN_THRESHOLD CONFIG_MIN_THRESHOLD
#define MAX_THRESHOLD CONFIG_MAX_THRESHOLD
#define REFRESH_TIME  CONFIG_REFRESH_TIME

#define ON_CHAIR_OF_CHAIR(my_cur_hall_value) ((my_cur_hall_value) < (MIN_THRESHOLD) || (my_cur_hall_value) > (MAX_THRESHOLD)  ? 1 : 0)

void read_hall_task(my_q_t* head);