#include "LinearWork.h"
#include "MyTimer.h"
#include "WorkObject.h"

static void countLettersInObject(WorkObject& object, size_t startLetter, size_t endLetter) {
    for (size_t i = startLetter; i < endLetter; i++) {
        object.result[object.string[i] - 'a']++;
    }
}

void linearWorker(std::queue<WorkObject>& objects, const size_t workerNumber, const size_t workersAmount, size_t elementsToProcess) {
    for (size_t i = 0; i < elementsToProcess; i++) {
        WorkObject object = objects.front();
        objects.pop();
        countLettersInObject(object,
                             object.string.length() * (double)workerNumber / workersAmount,
                             object.string.length() * (double)(workerNumber + 1) / workersAmount);
        objects.push(object);
    }
}

const int ITERATIONS = 5;
std::vector<WorkObject> initLinearWork(std::vector<std::string>& strings, const int workersAmount) {
    std::queue<WorkObject> objects;
    for (auto& string : strings) {
        objects.emplace(WorkObject(string));
    }
    MyTimer Timer;
    for (int i = 0; i < workersAmount; i++) {
        linearWorker(objects, i, workersAmount, strings.size());
    }
 //   std::cout << "Linear alg works for: " << Timer.elapsed() << " seconds" << std::endl;
    std::cout << Timer.elapsed() << " + ";

    std::vector<WorkObject> result;
    while (!objects.empty()) {
        result.push_back(objects.front());
        objects.pop();
    }
    return result;
}