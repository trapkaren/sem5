#ifndef LAB5_PROG_CONVEYORWORK_H
#define LAB5_PROG_CONVEYORWORK_H

#include <iostream>
#include <vector>
#include <queue>
#include <cstdlib>
#include <thread>
#include <mutex>
#include <unistd.h>
#include "WorkObject.h"

std::vector<WorkObject> initConveyorWork(std::vector<std::string>& strings, int workersAmount);


#endif //LAB5_PROG_CONVEYORWORK_H