#include <iostream>
#include <vector>
#include <queue>
#include <cstdlib>
#include "LinearWork.h"
#include "ConveyorWork.h"
#include "WorkObject.h"

int tests(size_t wordsAmount, size_t lettersInWord);

int main() {
    const int WORKERS_AMOUNT = 3;
    tests(3000, 1000);
    return 0;
}

int tests(size_t wordsAmount, size_t lettersInWord) {
    std::vector<std::string> strings;
    for (size_t i = 0; i < wordsAmount; i++) {
        std::string string;
        for (size_t j = 0; j < lettersInWord; j++) {
            string.push_back(rand() % LETTERS_IN_ENG_ALPHABET + 'a');
        }
        strings.emplace_back(string);
    }

    std::cout << "LINEAR" << std::endl;
    for (int i = 0; i < 5; i++) {
        std::vector<WorkObject> resultConsistent = initLinearWork(strings, 3);
    }

    std::cout << "\nPARALLEL" << std::endl;
    for (int i = 0; i < 5; i++) {
        std::vector<WorkObject> resultConveyor = initConveyorWork(strings, 3);
    }
    return EXIT_SUCCESS;
}