var turmerik_core;(()=>{"use strict";var e={570:(e,t)=>{Object.defineProperty(t,"__esModule",{value:!0}),t.findIdx=void 0,t.findIdx=(e,t)=>{const r={idx:-1,val:null};return e.find(((e,a)=>{const s=t(e,a);return s&&(r.idx=a,r.val=e),s})),r}},305:(e,t)=>{Object.defineProperty(t,"__esModule",{value:!0}),t.constSlice=t.normalizeSliceIndexes=void 0,t.normalizeSliceIndexes=e=>(0==e.totalCount?(e.startIdxVal=0,e.countVal=0):(e.startIdxVal=e.startIdx,e.countVal=e.count,e.startIdx>=0?e.count<0&&(e.countVal+=e.totalCount+1-e.startIdxVal):(e.startIdxVal+=e.totalCount,e.count<0&&(e.countVal*=-1,e.startIdxVal+=e.count))),e),t.constSlice=(e,r=0,a=-1)=>{const s=(0,t.normalizeSliceIndexes)({startIdx:r,count:a,totalCount:e.length,countVal:0,startIdxVal:0});return e.slice(s.startIdxVal,s.startIdxVal+s.countVal)}},920:function(e,t,r){var a=this&&this.__createBinding||(Object.create?function(e,t,r,a){void 0===a&&(a=r);var s=Object.getOwnPropertyDescriptor(t,r);s&&!("get"in s?!t.__esModule:s.writable||s.configurable)||(s={enumerable:!0,get:function(){return t[r]}}),Object.defineProperty(e,a,s)}:function(e,t,r,a){void 0===a&&(a=r),e[a]=t[r]}),s=this&&this.__setModuleDefault||(Object.create?function(e,t){Object.defineProperty(e,"default",{enumerable:!0,value:t})}:function(e,t){e.default=t}),l=this&&this.__importStar||function(e){if(e&&e.__esModule)return e;var t={};if(null!=e)for(var r in e)"default"!==r&&Object.prototype.hasOwnProperty.call(e,r)&&a(t,e,r);return s(t,e),t};Object.defineProperty(t,"__esModule",{value:!0}),t.arrays_slice_arr=t.arrays_find_idx=t.text_char=void 0,t.text_char=l(r(651)),t.arrays_find_idx=l(r(570)),t.arrays_slice_arr=l(r(305))},651:(e,t)=>{Object.defineProperty(t,"__esModule",{value:!0}),t.isValidCodeIdentifier=t.areAllUpperCaseLetters=t.areAllUpperCaseLettersOrNumbers=t.areAllLowerCaseLetters=t.areAllLowerCaseLettersOrNumbers=t.areAllLettersOrNumbers=t.areAllNumbers=t.areAllLetters=t.areAllWhitespaces=t.codeIdentifierRegex=t.upperCaseLettersRegex=t.upperCaseLettersOrNumbersRegex=t.lowerCaseLettersRegex=t.lowerCaseLettersOrNumbersRegex=t.lettersOrNumbersRegex=t.numbersRegex=t.lettersRegex=t.whitespacesRegex=void 0,t.whitespacesRegex=/^\s*$/,t.lettersRegex=/^[a-zA-Z]*$/,t.numbersRegex=/^[0-9]*$/,t.lettersOrNumbersRegex=/^[a-zA-Z0-9]*$/,t.lowerCaseLettersOrNumbersRegex=/^[a-z0-9]*$/,t.lowerCaseLettersRegex=/^[a-z]*$/,t.upperCaseLettersOrNumbersRegex=/^[A-Z0-9]*$/,t.upperCaseLettersRegex=/^[A-Z]*$/,t.codeIdentifierRegex=/^[a-zA-Z0-9_]*$/,t.areAllWhitespaces=e=>!!e.match(t.whitespacesRegex),t.areAllLetters=e=>!!e.match(t.lettersRegex),t.areAllNumbers=e=>!!e.match(t.numbersRegex),t.areAllLettersOrNumbers=e=>!!e.match(t.lettersOrNumbersRegex),t.areAllLowerCaseLettersOrNumbers=e=>!!e.match(t.lowerCaseLettersOrNumbersRegex),t.areAllLowerCaseLetters=e=>!!e.match(t.lowerCaseLettersRegex),t.areAllUpperCaseLettersOrNumbers=e=>!!e.match(t.upperCaseLettersOrNumbersRegex),t.areAllUpperCaseLetters=e=>!!e.match(t.upperCaseLettersRegex),t.isValidCodeIdentifier=e=>!!e.match(t.codeIdentifierRegex)}},t={},r=function r(a){var s=t[a];if(void 0!==s)return s.exports;var l=t[a]={exports:{}};return e[a].call(l.exports,l,l.exports,r),l.exports}(920);turmerik_core=r})();