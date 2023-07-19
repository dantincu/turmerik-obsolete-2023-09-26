import { normalizeSliceIndexes } from "../arrays/slice-arr";
import { areAllWhitespaces, areAllLettersOrNumbers } from "./char";

export interface Result {
  slicedStr: string;
  lastChar: string;
  nextChar: string;
  startIdx: number;
  endIdx: number;
}

export interface Args {
  inputStr: string;
  inputLen: number;
  startIdx: number;
  char: string;
  idx: number;
}

export const getArgs = (
  inputStr: string,
  inputLen: number,
  char: string,
  idx: number
) =>
  ({
    inputStr: inputStr,
    inputLen: inputLen,
    char: char,
    idx: idx,
  } as Args);

export const getStartIdx = (
  inputStr: string,
  inputLen: number,
  startIdx: number,
  endCharPredicate: (args: Args, stIdx: number) => number
) => {
  let endIdx = -1;
  let i = 0;
  let lenOfRest = inputLen - startIdx;

  while (i < lenOfRest) {
    const ch = inputStr[startIdx + 1];
    let inc = endCharPredicate(getArgs(inputStr, inputLen, ch, i), startIdx);

    if (isNaN(inc)) {
      endIdx = inputLen;
      break;
    } else {
      i += inc;

      if (inc <= 0) {
        endIdx = startIdx + i;
        break;
      }
    }
  }

  return endIdx;
};

export const slice = (
  inputStr: string,
  startCharPredicate: (args: Args) => number,
  endCharPredicate: (args: Args) => number,
  startIdx: number = 0,
  retIdxesOnly: boolean = false,
  callback: (result: Result) => void = null
) => {
  const inputLen = inputStr.length;
  startIdx = getStartIdx(inputStr, inputLen, startIdx, startCharPredicate);

  let endIdx = -1;
  let retStr: string = null;
  let lastChar: string = null;
  let nextChar: string = null;

  if (startIdx >= 0) {
    endIdx = getStartIdx(inputStr, inputLen, startIdx + 1, endCharPredicate);
  }

  if (endIdx >= 0 && !retIdxesOnly) {
    retStr = inputStr.substring(startIdx, endIdx);
    lastChar = inputStr[endIdx - 1];
    nextChar = inputStr[endIdx] ?? null;
  }

  const result: Result = {
    slicedStr: retStr,
    lastChar: lastChar,
    nextChar: nextChar,
    startIdx: startIdx,
    endIdx: endIdx,
  };

  callback?.call(result);
  return result;
};

export const constSlice = (
  inputStr: string,
  startIdx: number = 0,
  count: number = -1
) => {
  var args = normalizeSliceIndexes({
    startIdx: startIdx,
    count: count,
    totalCount: inputStr.length,
    countVal: 0,
    startIdxVal: 0,
  });

  const outArr = inputStr.slice(
    args.startIdxVal,
    args.startIdxVal + args.countVal
  );

  return outArr;
};

export const getNextWord = (
  inputStr: string,
  startIdx: number = 0,
  terminalChars: string = null,
  callback: (result: Result) => void = null
) => {
  terminalChars ??= "";

  const result = slice(
    inputStr,
    (args) => (args.idx < startIdx || areAllWhitespaces(args.char) ? 1 : 0),
    (args) =>
      areAllWhitespaces(args.char) || terminalChars.indexOf(args.char) >= 0
        ? 0
        : 1,
    startIdx,
    false,
    callback
  );

  return result;
};

export const getNextAlphaNumericWord = (
  inputStr: string,
  startIdx: number = 0,
  allowedChars: string = null,
  callback: (result: Result) => void = null
) => {
  allowedChars ??= "";

  const result = slice(
    inputStr,
    (args) =>
      args.idx >= startIdx && areAllLettersOrNumbers(args.char) ? 0 : 1,
    (args) =>
      areAllLettersOrNumbers(args.char) || allowedChars.indexOf(args.char) >= 0
        ? 1
        : 0,
    startIdx,
    false,
    callback
  );

  return result;
};

export const tryDigestStr = (
  inputStr: string,
  str: string,
  startIdx: number = 0,
  retIdxesOnly: boolean = false,
  callback: (result: Result) => void = null
) => {
  const strLen = str.length;
  const negStrLen = -strLen;

  const result = slice(
    inputStr,
    (args) => (constSlice(str, startIdx, negStrLen) == str ? 0 : 1),
    (args) => NaN,
    startIdx,
    retIdxesOnly,
    callback
  );

  return result;
};
