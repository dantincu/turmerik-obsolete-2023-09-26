import * as trmkrCore from "turmerik-core";

const funct = () => {
  console.log("asdfasdf");
};

const trmrk = {
  exp: {
    main: {
      funct: funct,
    },
  },
  lib: {
    trmkrCore,
  },
  imp: {},
};

(globalThis as any).trmrk = trmrk;

export default trmrk;
