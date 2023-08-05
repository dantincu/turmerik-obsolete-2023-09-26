"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.useWindowSize = void 0;
const react_1 = require("react");
const useWindowSize = () => {
    const [windowSize, setWindowSize] = (0, react_1.useState)({
        width: undefined,
        height: undefined,
    });
    (0, react_1.useEffect)(() => {
        function handleResize() {
            setWindowSize({
                width: window.innerWidth,
                height: window.innerHeight,
            });
        }
        window.addEventListener("resize", handleResize);
        // Call handler right away so state gets updated with initial window size
        handleResize();
        // Don't forget to remove event listener on cleanup
        return () => window.removeEventListener("resize", handleResize);
    }, []);
    return windowSize;
};
exports.useWindowSize = useWindowSize;
