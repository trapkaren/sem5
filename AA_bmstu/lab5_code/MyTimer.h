#ifndef LAB5_PROG_MYTIMER_H
#define LAB5_PROG_MYTIMER_H

#include <chrono>

class MyTimer {
private:
    using clock_t = std::chrono::high_resolution_clock;
    using second_t = std::chrono::duration<double, std::ratio<1>>;

    std::chrono::time_point<clock_t> m_beg;

public:
    MyTimer();

    void reset();

    double elapsed() const;
};

#endif //LAB5_PROG_MYTIMER_H