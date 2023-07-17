/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ([
/* 0 */,
/* 1 */
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   constSlice: () => (/* binding */ constSlice),
/* harmony export */   getArgs: () => (/* binding */ getArgs),
/* harmony export */   getEndIdx: () => (/* binding */ getEndIdx),
/* harmony export */   getNextAlphaNumericWord: () => (/* binding */ getNextAlphaNumericWord),
/* harmony export */   getNextWord: () => (/* binding */ getNextWord),
/* harmony export */   getStartIdx: () => (/* binding */ getStartIdx),
/* harmony export */   slice: () => (/* binding */ slice),
/* harmony export */   tryDigestStr: () => (/* binding */ tryDigestStr)
/* harmony export */ });
/* harmony import */ var _arrays_slice_arr__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(2);
/* harmony import */ var _char__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(3);


const getArgs = (inputStr, inputLen, char, idx) => ({
    inputStr: inputStr,
    inputLen: inputLen,
    char: char,
    idx: idx,
});
const getStartIdx = (inputStr, inputLen, startCharPredicate) => {
    let startIdx = -1;
    let i = 0;
    while (i < inputLen) {
        const ch = inputStr[i];
        const inc = startCharPredicate(getArgs(inputStr, inputLen, ch, i));
        i += inc;
        if (inc <= 0) {
            startIdx = i;
            break;
        }
    }
    return startIdx;
};
const getEndIdx = (inputStr, inputLen, startIdx, endCharPredicate) => {
    let endIdx = -1;
    let i = 0;
    startIdx++;
    let lenOfRest = inputLen - startIdx;
    while (i < lenOfRest) {
        const ch = inputStr[startIdx + 1];
        let inc = endCharPredicate(getArgs(inputStr, inputLen, ch, i), startIdx);
        if (isNaN(inc)) {
            endIdx = inputLen;
            break;
        }
        else {
            i += inc;
            if (inc <= 0) {
                endIdx = startIdx + i;
                break;
            }
        }
    }
    return endIdx;
};
const slice = (inputStr, startCharPredicate, endCharPredicate, retIdxesOnly = false, callback = null) => {
    const inputLen = inputStr.length;
    const startIdx = getStartIdx(inputStr, inputLen, startCharPredicate);
    let endIdx = -1;
    let retStr = null;
    let lastChar = null;
    let nextChar = null;
    if (startIdx >= 0) {
        endIdx = getEndIdx(inputStr, inputLen, startIdx, endCharPredicate);
    }
    if (endIdx >= 0 && !retIdxesOnly) {
        retStr = inputStr.substring(startIdx, endIdx);
        lastChar = inputStr[endIdx - 1];
        nextChar = inputStr[endIdx] ?? null;
    }
    const result = {
        slicedStr: retStr,
        lastChar: lastChar,
        nextChar: nextChar,
        startIdx: startIdx,
        endIdx: endIdx,
    };
    callback?.call(result);
    return result;
};
const constSlice = (inputStr, startIdx = 0, count = -1) => {
    var args = (0,_arrays_slice_arr__WEBPACK_IMPORTED_MODULE_0__.normalizeSliceIndexes)({
        startIdx: startIdx,
        count: count,
        totalCount: inputStr.length,
        countVal: 0,
        startIdxVal: 0,
    });
    const outArr = inputStr.slice(args.startIdxVal, args.startIdxVal + args.countVal);
    return outArr;
};
const getNextWord = (inputStr, startIdx = 0, terminalChars = "", callback = null) => {
    terminalChars ??= "";
    const result = slice(inputStr, (args) => (args.idx < startIdx || (0,_char__WEBPACK_IMPORTED_MODULE_1__.areAllWhitespaces)(args.char) ? 1 : 0), (args, stIdx) => (0,_char__WEBPACK_IMPORTED_MODULE_1__.areAllWhitespaces)(args.char) || terminalChars.indexOf(args.char) >= 0
        ? 0
        : 1, false, callback);
    return result;
};
const getNextAlphaNumericWord = (inputStr, startIdx = 0, allowedChars = "", callback = null) => {
    allowedChars ??= "";
    const result = slice(inputStr, (args) => args.idx >= startIdx && (0,_char__WEBPACK_IMPORTED_MODULE_1__.areAllLettersOrNumbers)(args.char) ? 0 : 1, (args, stIdx) => (0,_char__WEBPACK_IMPORTED_MODULE_1__.areAllLettersOrNumbers)(args.char) || allowedChars.indexOf(args.char) >= 0
        ? 1
        : 0, false, callback);
    return result;
};
const tryDigestStr = (inputStr, str, startIdx = 0, retIdxesOnly = false, callback = null) => {
    const strLen = str.length;
    const negStrLen = -strLen;
    const result = slice(inputStr, (args) => (constSlice(str, startIdx, negStrLen) == str ? 0 : 1), (args, stIdx) => NaN, retIdxesOnly, callback);
    return result;
};


/***/ }),
/* 2 */
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
/* 3 */
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
/* harmony import */ var _text_slice_str__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(1);

let str = (0,_text_slice_str__WEBPACK_IMPORTED_MODULE_0__.slice)("asdfasdf", (args) => 3 - args.idx, (args, stIdx) => NaN);
console.log("sliceStr", str);
str = (0,_text_slice_str__WEBPACK_IMPORTED_MODULE_0__.slice)("asdfasdf", (args) => 3 - args.idx, (args, stIdx) => 3 - args.idx);
console.log("sliceStr", str);

})();

/******/ })()
;