var turmerik_jint_fs_notes;(()=>{"use strict";var t={d:(e,r)=>{for(var a in r)t.o(r,a)&&!t.o(e,a)&&Object.defineProperty(e,a,{enumerable:!0,get:r[a]})},o:(t,e)=>Object.prototype.hasOwnProperty.call(t,e),r:t=>{"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(t,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(t,"__esModule",{value:!0})}},e={};t.r(e),t.d(e,{default:()=>P});var r={};t.r(r),t.d(r,{constSlice:()=>c,normalizeSliceIndexes:()=>o});var a={};t.r(a),t.d(a,{findIdx:()=>u});var l={};t.r(l),t.d(l,{areAllLetters:()=>V,areAllLettersOrNumbers:()=>A,areAllLowerCaseLetters:()=>N,areAllLowerCaseLettersOrNumbers:()=>v,areAllNumbers:()=>C,areAllUpperCaseLetters:()=>O,areAllUpperCaseLettersOrNumbers:()=>L,areAllWhitespaces:()=>p,codeIdentifierRegex:()=>b,isValidCodeIdentifier:()=>S,lettersOrNumbersRegex:()=>m,lettersRegex:()=>i,lowerCaseLettersOrNumbersRegex:()=>g,lowerCaseLettersRegex:()=>h,numbersRegex:()=>x,upperCaseLettersOrNumbersRegex:()=>f,upperCaseLettersRegex:()=>I,whitespacesRegex:()=>d});var n={};t.r(n),t.d(n,{constSlice:()=>k,getArgs:()=>y,getEndIdx:()=>$,getNextAlphaNumericWord:()=>w,getNextWord:()=>j,getStartIdx:()=>R,slice:()=>_,tryDigestStr:()=>z});var s={};t.r(s),t.d(s,{char:()=>l,findIdx:()=>a,sliceArr:()=>r,sliceStr:()=>n});const o=t=>(0==t.totalCount?(t.startIdxVal=0,t.countVal=0):(t.startIdxVal=t.startIdx,t.countVal=t.count,t.startIdx>=0?t.count<0&&(t.countVal+=t.totalCount+1-t.startIdxVal):(t.startIdxVal+=t.totalCount,t.count<0&&(t.countVal*=-1,t.startIdxVal+=t.count))),t),c=(t,e=0,r=-1)=>{var a=o({startIdx:e,count:r,totalCount:t.length,countVal:0,startIdxVal:0});return t.slice(a.startIdxVal,a.startIdxVal+a.countVal)},u=(t,e)=>{const r={idx:-1,val:null};return t.find(((t,a)=>{const l=e(t,a);return l&&(r.idx=a,r.val=t),l})),r},d=/^\s$/,i=/^[a-zA-Z]$/,x=/^[0-9]$/,m=/^[a-zA-Z0-9]$/,g=/^[a-z0-9]$/,h=/^[a-z]$/,f=/^[A-Z0-9]$/,I=/^[A-Z]$/,b=/^[a-zA-Z0-9_]$/,p=t=>!!t.match(d),V=t=>!!t.match(i),C=t=>!!t.match(x),A=t=>!!t.match(m),v=t=>!!t.match(g),N=t=>!!t.match(h),L=t=>!!t.match(f),O=t=>!!t.match(I),S=t=>!!t.match(b),y=(t,e,r,a)=>({inputStr:t,inputLen:e,char:r,idx:a}),R=(t,e,r)=>{let a=-1,l=0;for(;l<e;){const n=t[l],s=r(y(t,e,n,l));if(l+=s,s<=0){a=l;break}}return a},$=(t,e,r,a)=>{let l=-1,n=0,s=e-++r;for(;n<s;){const s=t[r+1];let o=a(y(t,e,s,n),r);if(isNaN(o)){l=e;break}if(n+=o,o<=0){l=r+n;break}}return l},_=(t,e,r,a=!1,l=null)=>{const n=t.length,s=R(t,n,e);let o=-1,c=null,u=null,d=null;s>=0&&(o=$(t,n,s,r)),o>=0&&!a&&(c=t.substring(s,o),u=t[o-1],d=t[o]??null);const i={slicedStr:c,lastChar:u,nextChar:d,startIdx:s,endIdx:o};return l?.call(i),i},k=(t,e=0,r=-1)=>{var a=o({startIdx:e,count:r,totalCount:t.length,countVal:0,startIdxVal:0});return t.slice(a.startIdxVal,a.startIdxVal+a.countVal)},j=(t,e=0,r=null,a=null)=>(r??="",_(t,(t=>t.idx<e||p(t.char)?1:0),((t,e)=>p(t.char)||r.indexOf(t.char)>=0?0:1),!1,a)),w=(t,e=0,r=null,a=null)=>(r??="",_(t,(t=>t.idx>=e&&A(t.char)?0:1),((t,e)=>A(t.char)||r.indexOf(t.char)>=0?1:0),!1,a)),z=(t,e,r=0,a=!1,l=null)=>{const n=-e.length;return _(t,(t=>k(e,r,n)==e?0:1),((t,e)=>NaN),a,l)},Z={exp:{main:{funct:()=>{console.log("asdfasdf")}}},lib:{trmkrCore:s},imp:{}};globalThis.trmrk=Z;const P=Z;turmerik_jint_fs_notes=e})();