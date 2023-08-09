"use client"; // This is a client component 👈🏽

import { detect_mobile } from "@turmerik/next-react";
import { Container } from '@mui/material';
import { useSearchParams } from "next/navigation"
import "./ReadonlyPane.scss"

import { ReadonlyPaneProps } from './ReadonlyPaneProps';
import { getPaneCssClassName } from '../classNames';

export default function ReadonlyPane(
  props: ReadonlyPaneProps
) {
  const isMobile = detect_mobile.useIsMobile()
  const urlQuery = useSearchParams();

  return (
    <Container className={getPaneCssClassName("readonly", props.isVisible)}></Container>
  );
}