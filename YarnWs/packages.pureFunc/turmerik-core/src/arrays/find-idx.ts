export interface IdxKvp<T> {
  idx: number;
  val: T | null;
}

export const findIdx = <T>(
  inputArr: T[],
  predicate: (item: T, idx: number) => boolean
) => {
  const result: IdxKvp<T> = {
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
