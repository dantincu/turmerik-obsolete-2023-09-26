import { slice } from "./src/text/slice-str";

let str = slice(
  "asdfasdf",
  (args) => 3 - args.idx,
  (args, stIdx) => NaN
);

console.log("sliceStr", str);

str = slice(
  "asdfasdf",
  (args) => 3 - args.idx,
  (args, stIdx) => 3 - args.idx
);

console.log("sliceStr", str);
