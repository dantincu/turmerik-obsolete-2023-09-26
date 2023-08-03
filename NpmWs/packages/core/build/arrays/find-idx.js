"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.findIdx = void 0;
const findIdx = (inputArr, predicate) => {
    const result = {
        idx: -1,
        val: null,
    };
    inputArr.find((item, idx) => {
        const retVal = predicate(item, idx);
        if (retVal) {
            result.idx = idx;
            result.val = item;
        }
        return retVal;
    });
    return result;
};
exports.findIdx = findIdx;
