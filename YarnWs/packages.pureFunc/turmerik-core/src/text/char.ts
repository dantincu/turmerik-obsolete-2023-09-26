export const whitespacesRegex = /^\s$/;

export const lettersRegex = /^[a-zA-Z]$/;
export const numbersRegex = /^[0-9]$/;
export const lettersOrNumbersRegex = /^[a-zA-Z0-9]$/;

export const lowerCaseLettersOrNumbersRegex = /^[a-z0-9]$/;
export const lowerCaseLettersRegex = /^[a-z]$/;

export const upperCaseLettersOrNumbersRegex = /^[A-Z0-9]$/;
export const upperCaseLettersRegex = /^[A-Z]$/;

export const codeIdentifierRegex = /^[a-zA-Z0-9_]$/;

export const areAllWhitespaces = (str: string) => !!str.match(whitespacesRegex);
export const areAllLetters = (str: string) => !!str.match(lettersRegex);
export const areAllNumbers = (str: string) => !!str.match(numbersRegex);

export const areAllLettersOrNumbers = (str: string) =>
  !!str.match(lettersOrNumbersRegex);

export const areAllLowerCaseLettersOrNumbers = (str: string) =>
  !!str.match(lowerCaseLettersOrNumbersRegex);

export const areAllLowerCaseLetters = (str: string) =>
  !!str.match(lowerCaseLettersRegex);

export const areAllUpperCaseLettersOrNumbers = (str: string) =>
  !!str.match(upperCaseLettersOrNumbersRegex);

export const areAllUpperCaseLetters = (str: string) =>
  !!str.match(upperCaseLettersRegex);

export const isValidCodeIdentifier = (str: string) =>
  !!str.match(codeIdentifierRegex);
