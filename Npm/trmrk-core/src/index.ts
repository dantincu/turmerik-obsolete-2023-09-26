import * as main from "./main";

declare const globalThis;

globalThis.root = {
  main: main,
  cfg: {
    mySetting: 123,
  },
};
