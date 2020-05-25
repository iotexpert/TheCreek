
struct bcm2835_i2cbb {
    uint8_t address; // 7 bit address
    uint8_t sda; // pin used for sda coresponds to gpio
    uint8_t scl; // clock
    uint32_t clock_delay; // proporional to bus speed
    uint32_t timeout; //
};

// *****************************************************************************
// open bus, sets structure and initialises GPIO
// The scl and sda line are set to be always 0 (low) output, when a high is
// required they are set to be an input.
// *****************************************************************************
int bcm2835_i2cbb_open(struct bcm2835_i2cbb *,  uint8_t ,   uint8_t , uint8_t ,  int32_t , uint32_t );

void bcm2835_i2cbb_bitdelay(uint32_t );
void bcm2835_i2cbb_sclH(struct bcm2835_i2cbb *);
void bcm2835_i2cbb_sclL(struct bcm2835_i2cbb *);
void bcm2835_i2cbb_sdaH(struct bcm2835_i2cbb *);
void bcm2835_i2cbb_sdaL(struct bcm2835_i2cbb *);
int bcm2835_i2cbb_free(struct bcm2835_i2cbb *);
int bcm2835_i2cbb_start(struct bcm2835_i2cbb *);
int bcm2835_i2cbb_restart(struct bcm2835_i2cbb *);
void bcm2835_i2cbb_stop(struct bcm2835_i2cbb *);
int bcm2835_i2cbb_send(struct bcm2835_i2cbb *, uint8_t );
uint8_t bcm2835_i2cbb_read(struct bcm2835_i2cbb *, uint8_t );
void bcm8235_i2cbb_putc(struct bcm2835_i2cbb *, uint8_t );
void bcm8235_i2cbb_puts(struct bcm2835_i2cbb *, uint8_t *, uint32_t );
uint8_t bcm8235_i2cbb_getc(struct bcm2835_i2cbb *);
void bcm8235_i2cbb_gets(struct bcm2835_i2cbb *, uint8_t *, uint32_t );
void bcm8235_i2cbb_discover(struct bcm2835_i2cbb *, uint8_t , uint8_t );
