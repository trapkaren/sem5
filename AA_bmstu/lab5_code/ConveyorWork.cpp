#include "ConveyorWork.h"
#include "MyTimer.h"

std::mutex allThreadsLock;

static void countLettersInObject (WorkObject& object, size_t startLetter, size_t endLetter) {
    for (size_t i = startLetter; i < endLetter; i++) {
        object.result[object.string[i] - 'a']++;
    }
}

void threadWork(const int threadNumber, const int threadsAmount, std::queue<WorkObject>& threadsQueue, std::queue<WorkObject>& nextThreadQueue, size_t objectsToProcess,
                std::mutex& threadsMutex, std::mutex& nextThreadsMutex) {
    MyTimer timerThreadWork;
    size_t sleepTime;
    size_t processed = 0;

   // std::cout << "Thread # " << threadNumber << " worked for " << timerThreadWork.elapsed() << " seconds" << std::endl << "\n";

    while(processed != objectsToProcess) {
        if (threadsQueue.size()) {
            WorkObject object = threadsQueue.front();

            threadsMutex.lock();
            threadsQueue.pop();
            threadsMutex.unlock();

            countLettersInObject(object, object.string.length() * (double)threadNumber / threadsAmount, object.string.length() * (double)(threadNumber + 1) / threadsAmount);

            nextThreadsMutex.lock();
            nextThreadQueue.push(object);
            nextThreadsMutex.unlock();

            processed++;
        } else {
            usleep(1000);
            sleepTime += 1;
        }
    }

//    std::cout << "Thread # " << threadNumber << " sleeped for " << sleepTime << " milliseconds" << std::endl << "\n";

 /*   allThreadsLock.lock();
    std::cout << "Thread # " << threadNumber << " worked for " << timerThreadWork.elapsed() << " seconds" << std::endl;
    std::cout << "Thread # " << threadNumber << " sleeped for " << sleepTime << " milliseconds" << std::endl;
    allThreadsLock.unlock(); */
}

std::vector<WorkObject> initConveyorWork(std::vector<std::string>& strings, const int workersAmount) {
    std::queue<WorkObject> completedObjects;
    std::vector<std::queue<WorkObject>> workersQueues(workersAmount);
    std::vector<std::thread> threads;
    for (auto& string : strings) {
        workersQueues[0].push(WorkObject(string));
    }

    std::vector<std::mutex> mutexes(workersAmount + 1);

    MyTimer timerAllWork;
    for (size_t i = 0; i < workersAmount; i++) {
        threads.emplace_back(std::thread(threadWork, i, workersAmount, std::ref(workersQueues[i]),
                                         i == workersAmount - 1 ? std::ref(completedObjects) : std::ref(
                                                 workersQueues[i + 1]), strings.size(), std::ref(mutexes[i]),
                                         std::ref(mutexes[i + 1])));
    }

    for (size_t i = 0; i < workersAmount; i++) {
        threads[i].join();
    }

    //std::cout << "All Conveyor worked for: " << timerAllWork.elapsed() << " seconds" << std::endl;
    std::cout << timerAllWork.elapsed() << " + ";
    std::vector<WorkObject> result;
    while (!completedObjects.empty()) {
        result.push_back(completedObjects.front());
        completedObjects.pop();
    }
    return result;
}

// mutex