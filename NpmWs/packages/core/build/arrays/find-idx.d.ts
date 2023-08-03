export interface IdxKvp<T> {
    idx: number;
    val: T | null;
}
export declare const findIdx: <T>(inputArr: T[], predicate: (item: T, idx: number) => boolean) => IdxKvp<T>;
