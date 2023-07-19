import { slice } from "./src/text/slice-str";

let str = slice(
  "asdfasdf",
  (args) => 3 - args.idx,
  (args) => NaN
);

console.log("sliceStr", str);

str = slice(
  "asdfasdf",
  (args) => 3 - args.idx,
  (args) => 3 - args.idx
);

console.log("sliceStr", str);
