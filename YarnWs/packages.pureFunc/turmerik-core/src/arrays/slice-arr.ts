export interface NormSliceIdxesArgs {
  startIdx: number;
  count: number;
  totalCount: number;
  startIdxVal: number;
  countVal: number;
}

export const normalizeSliceIndexes = (args: NormSliceIdxesArgs) => {
  if (args.totalCount == 0) {
    args.startIdxVal = 0;
    args.countVal = 0;
  } else {
    args.startIdxVal = args.startIdx;
    args.countVal = args.count;

    if (args.startIdx >= 0) {
      if (args.count < 0) {
        args.countVal += args.totalCount + 1 - args.startIdxVal;
      }
    } else {
      args.startIdxVal += args.totalCount;

      if (args.count < 0) {
        args.countVal *= -1;
        args.startIdxVal += args.count;
      }
    }
  }

  return args;
};

export const constSlice = <T>(
  inputArr: T[],
  startIdx: number = 0,
  count: number = -1
) => {
  var args = normalizeSliceIndexes({
    startIdx: startIdx,
    count: count,
    totalCount: inputArr.length,
    countVal: 0,
    startIdxVal: 0,
  });

  const outArr = inputArr.slice(
    args.startIdxVal,
    args.startIdxVal + args.countVal
  );

  return outArr;
};
