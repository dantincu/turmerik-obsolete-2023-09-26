/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ([
/* 0 */,
/* 1 */
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var turmerik_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(2);

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
        trmkrCore: turmerik_core__WEBPACK_IMPORTED_MODULE_0__,
    },
    imp: {},
};
globalThis.trmrk = trmrk;
/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (trmrk);


/***/ }),
/* 2 */
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   char: () => (/* reexport module object */ _src_text_char__WEBPACK_IMPORTED_MODULE_2__),
/* harmony export */   findIdx: () => (/* reexport module object */ _src_arrays_find_idx__WEBPACK_IMPORTED_MODULE_1__),
/* harmony export */   sliceArr: () => (/* reexport module object */ _src_arrays_slice_arr__WEBPACK_IMPORTED_MODULE_0__)
/* harmony export */ });
/* harmony import */ var _src_arrays_slice_arr__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(3);
/* harmony import */ var _src_arrays_find_idx__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var _src_text_char__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(5);





/***/ }),
/* 3 */
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   constSlice: () => (/* binding */ constSlice),
/* harmony export */   normalizeSliceIndexes: () => (/* binding */ normalizeSliceIndexes)
/* harmony export */ });
const normalizeSliceIndexes = (args) => {
    if (args.totalCount == 0) {
        args.startIdxVal = 0;
        args.countVal = 0;
    }
    else {
        args.startIdxVal = args.startIdx;
        args.countVal = args.count;
        if (args.startIdx >= 0) {
            if (args.count < 0) {
                args.countVal += args.totalCount + 1 - args.startIdxVal;
            }
        }
        else {
            args.startIdxVal += args.totalCount;
            if (args.count < 0) {
                args.countVal *= -1;
                args.startIdxVal += args.count;
            }
        }
    }
    return args;
};
const constSlice = (inputArr, startIdx = 0, count = -1) => {
    var args = normalizeSliceIndexes({
        startIdx: startIdx,
        count: count,
        totalCount: inputArr.length,
        countVal: 0,
        startIdxVal: 0,
    });
    const outArr = inputArr.slice(args.startIdxVal, args.startIdxVal + args.countVal);
    return outArr;
};


/***/ }),
/* 4 */
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   findIdx: () => (/* binding */ findIdx)
/* harmony export */ });
const findIdx = (inputArr, predicate) => {
    const result = {
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


/***/ }),
/* 5 */
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   areAllLetters: () => (/* binding */ areAllLetters),
/* harmony export */   areAllLettersOrNumbers: () => (/* binding */ areAllLettersOrNumbers),
/* harmony export */   areAllLowerCaseLetters: () => (/* binding */ areAllLowerCaseLetters),
/* harmony export */   areAllLowerCaseLettersOrNumbers: () => (/* binding */ areAllLowerCaseLettersOrNumbers),
/* harmony export */   areAllNumbers: () => (/* binding */ areAllNumbers),
/* harmony export */   areAllUpperCaseLetters: () => (/* binding */ areAllUpperCaseLetters),
/* harmony export */   areAllUpperCaseLettersOrNumbers: () => (/* binding */ areAllUpperCaseLettersOrNumbers),
/* harmony export */   areAllWhitespaces: () => (/* binding */ areAllWhitespaces),
/* harmony export */   codeIdentifierRegex: () => (/* binding */ codeIdentifierRegex),
/* harmony export */   isValidCodeIdentifier: () => (/* binding */ isValidCodeIdentifier),
/* harmony export */   lettersOrNumbersRegex: () => (/* binding */ lettersOrNumbersRegex),
/* harmony export */   lettersRegex: () => (/* binding */ lettersRegex),
/* harmony export */   lowerCaseLettersOrNumbersRegex: () => (/* binding */ lowerCaseLettersOrNumbersRegex),
/* harmony export */   lowerCaseLettersRegex: () => (/* binding */ lowerCaseLettersRegex),
/* harmony export */   numbersRegex: () => (/* binding */ numbersRegex),
/* harmony export */   upperCaseLettersOrNumbersRegex: () => (/* binding */ upperCaseLettersOrNumbersRegex),
/* harmony export */   upperCaseLettersRegex: () => (/* binding */ upperCaseLettersRegex),
/* harmony export */   whitespacesRegex: () => (/* binding */ whitespacesRegex)
/* harmony export */ });
const whitespacesRegex = /^\s$/;
const lettersRegex = /^[a-zA-Z]$/;
const numbersRegex = /^[0-9]$/;
const lettersOrNumbersRegex = /^[a-zA-Z0-9]$/;
const lowerCaseLettersOrNumbersRegex = /^[a-z0-9]$/;
const lowerCaseLettersRegex = /^[a-z]$/;
const upperCaseLettersOrNumbersRegex = /^[A-Z0-9]$/;
const upperCaseLettersRegex = /^[A-Z]$/;
const codeIdentifierRegex = /^[a-zA-Z0-9_]$/;
const areAllWhitespaces = (str) => !!str.match(whitespacesRegex);
const areAllLetters = (str) => !!str.match(lettersRegex);
const areAllNumbers = (str) => !!str.match(numbersRegex);
const areAllLettersOrNumbers = (str) => !!str.match(lettersOrNumbersRegex);
const areAllLowerCaseLettersOrNumbers = (str) => !!str.match(lowerCaseLettersOrNumbersRegex);
const areAllLowerCaseLetters = (str) => !!str.match(lowerCaseLettersRegex);
const areAllUpperCaseLettersOrNumbers = (str) => !!str.match(upperCaseLettersOrNumbersRegex);
const areAllUpperCaseLetters = (str) => !!str.match(upperCaseLettersRegex);
const isValidCodeIdentifier = (str) => !!str.match(codeIdentifierRegex);


/***/ })
/******/ 	]);
/************************************************************************/
/******/ 	// The module cache
/******/ 	var __webpack_module_cache__ = {};
/******/ 	
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/ 		// Check if module is in cache
/******/ 		var cachedModule = __webpack_module_cache__[moduleId];
/******/ 		if (cachedModule !== undefined) {
/******/ 			return cachedModule.exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = __webpack_module_cache__[moduleId] = {
/******/ 			// no module.id needed
/******/ 			// no module.loaded needed
/******/ 			exports: {}
/******/ 		};
/******/ 	
/******/ 		// Execute the module function
/******/ 		__webpack_modules__[moduleId](module, module.exports, __webpack_require__);
/******/ 	
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/define property getters */
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => (Object.prototype.hasOwnProperty.call(obj, prop))
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	(() => {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = (exports) => {
/******/ 			if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 			}
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
/******/ 		};
/******/ 	})();
/******/ 	
/************************************************************************/
var __webpack_exports__ = {};
// This entry need to be wrapped in an IIFE because it need to be isolated against other modules in the chunk.
(() => {
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _index__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(1);

_index__WEBPACK_IMPORTED_MODULE_0__["default"].exp.main.funct();

})();

/******/ })()
;