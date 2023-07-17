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
  startCharPredicate: (args: Args) => number
) => {
  let startIdx = -1;
  let i = 0;

  while (i < inputLen) {
    const ch = inputStr[i];
    const inc = startCharPredicate(getArgs(inputStr, inputLen, ch, i));
    i += inc;

    if (inc <= 0) {
      startIdx = i;
      break;
    }
  }

  return startIdx;
};

export const getEndIdx = (
  inputStr: string,
  inputLen: number,
  startIdx: number,
  endCharPredicate: (args: Args, stIdx: number) => number
) => {
  let endIdx = -1;
  let i = 0;
  startIdx++;
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
  endCharPredicate: (args: Args, stIdx: number) => number,
  retIdxesOnly: boolean = false,
  callback: (result: Result) => void = null
) => {
  const inputLen = inputStr.length;
  const startIdx = getStartIdx(inputStr, inputLen, startCharPredicate);

  let endIdx = -1;
  let retStr: string = null;
  let lastChar: string = null;
  let nextChar: string = null;

  if (startIdx >= 0) {
    endIdx = getEndIdx(inputStr, inputLen, startIdx, endCharPredicate);
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
    (args, stIdx) =>
      areAllWhitespaces(args.char) || terminalChars.indexOf(args.char) >= 0
        ? 0
        : 1,
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
    (args, stIdx) =>
      areAllLettersOrNumbers(args.char) || allowedChars.indexOf(args.char) >= 0
        ? 1
        : 0,
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
    (args, stIdx) => NaN,
    retIdxesOnly,
    callback
  );

  return result;
};
