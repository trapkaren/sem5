#ifndef LAB5_PROG_WORKOBJECT_H
#define LAB5_PROG_WORKOBJECT_H

#include <string>
#include <vector>


const int LETTERS_IN_ENG_ALPHABET = 26;
struct WorkObject {
    std::string string;
    std::vector<int> result;

    WorkObject(size_t lettersInWords) {
        for (size_t i = 0; i < lettersInWords; i++) {
            string.push_back(rand() % LETTERS_IN_ENG_ALPHABET + 'a');
        }
        for (int i = 0; i < LETTERS_IN_ENG_ALPHABET; i++) {
            result.push_back(0);
        }
    }

    WorkObject(std::string& string) : string(string){
        for (int i = 0; i < LETTERS_IN_ENG_ALPHABET; i++) {
            result.push_back(0);
        }
    }

    bool operator==(const WorkObject& rh) const {
        return result == rh.result;
    }

    bool operator!=(const WorkObject& rh) const {
        return result != rh.result;
    }
};

#endif //LAB5_PROG_WORKOBJECT_H