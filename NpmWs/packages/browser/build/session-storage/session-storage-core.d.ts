export declare const normalizeKey: (key: string | string[]) => string;
export declare const getOrCreate: <T>(key: string[] | string, factory: () => [boolean, T]) => [boolean, T, boolean];
export declare const getOrCreateAsync: <T>(key: string[] | string, factory: () => Promise<[boolean, T]>) => Promise<[boolean, T, boolean]>;
