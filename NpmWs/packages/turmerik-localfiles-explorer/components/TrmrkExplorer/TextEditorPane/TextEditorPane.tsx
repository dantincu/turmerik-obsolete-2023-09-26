"use client"; // This is a client component 👈🏽

import { detect_mobile } from "@turmerik/next-react";
import { Container } from '@mui/material';
import { useSearchParams } from "next/navigation"
import "./TextEditorPane.scss"

import { TextEditorPaneProps } from './TextEditorPaneProps';
import { getPaneCssClassName } from '../classNames';

export default function TextEditorPane(
  props: TextEditorPaneProps
) {
  const isMobile = detect_mobile.useIsMobile()
  const urlQuery = useSearchParams();

  return (
    <Container className={getPaneCssClassName("text-editor", props.isVisible)}></Container>
  );
}