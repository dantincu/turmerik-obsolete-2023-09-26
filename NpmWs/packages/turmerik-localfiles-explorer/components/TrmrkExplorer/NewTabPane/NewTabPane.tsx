"use client"; // This is a client component 👈🏽

import { detect_mobile } from "@turmerik/next-react";
import { Container } from '@mui/material';
import { useSearchParams } from "next/navigation"
import "./NewTabPane.scss"

import { NewTabPaneProps } from './NewTabPaneProps';
import { getPaneCssClassName } from '../classNames';

export default function NewTabPane(
  props: NewTabPaneProps
) {
  const isMobile = detect_mobile.useIsMobile()
  const urlQuery = useSearchParams();

  return (
    <Container className={getPaneCssClassName("home", props.isVisible)}></Container>
  );
}