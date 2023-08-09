"use client"; // This is a client component 👈🏽

import { detect_mobile } from "@turmerik/next-react";
import { Container } from '@mui/material';
import { useSearchParams } from "next/navigation"
import "./PreviewPane.scss"

import { PreviewPaneProps } from './PreviewPaneProps';
import { getPaneCssClassName } from '../classNames';

export default function PreviewPane(
  props: PreviewPaneProps
) {
  const isMobile = detect_mobile.useIsMobile()
  const urlQuery = useSearchParams();

  return (
    <Container className={getPaneCssClassName("preview", props.isVisible)}></Container>
  );
}