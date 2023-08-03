export interface NormSliceIdxesArgs {
    startIdx: number;
    count: number;
    totalCount: number;
    startIdxVal: number;
    countVal: number;
}
export declare const normalizeSliceIndexes: (args: NormSliceIdxesArgs) => NormSliceIdxesArgs;
export declare const constSlice: <T>(inputArr: T[], startIdx?: number, count?: number) => T[];
