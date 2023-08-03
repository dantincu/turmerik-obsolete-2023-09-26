"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.constSlice = exports.normalizeSliceIndexes = void 0;
const normalizeSliceIndexes = (args) => {
    if (args.totalCount == 0) {
        args.startIdxVal = 0;
        args.countVal = 0;
    }
    else {
        args.startIdxVal = args.startIdx;
        args.countVal = args.count;
        if (args.startIdx >= 0) {
            if (args.count < 0) {
                args.countVal += args.totalCount + 1 - args.startIdxVal;
            }
        }
        else {
            args.startIdxVal += args.totalCount;
            if (args.count < 0) {
                args.countVal *= -1;
                args.startIdxVal += args.count;
            }
        }
    }
    return args;
};
exports.normalizeSliceIndexes = normalizeSliceIndexes;
const constSlice = (inputArr, startIdx = 0, count = -1) => {
    const args = (0, exports.normalizeSliceIndexes)({
        startIdx: startIdx,
        count: count,
        totalCount: inputArr.length,
        countVal: 0,
        startIdxVal: 0,
    });
    const outArr = inputArr.slice(args.startIdxVal, args.startIdxVal + args.countVal);
    return outArr;
};
exports.constSlice = constSlice;
