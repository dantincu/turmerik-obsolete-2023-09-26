"use client"; // This is a client component 👈🏽

import { detect_mobile } from "@turmerik/next-react";
import { Container } from '@mui/material';
import { useSearchParams } from "next/navigation"
import "./DirHcyPane.scss"

import { DirHcyPaneProps } from './DirHcyPaneProps';
import { getPaneCssClassName } from '../classNames';

export default function DirHcyPane(
  props: DirHcyPaneProps
) {
  const isMobile = detect_mobile.useIsMobile()
  const urlQuery = useSearchParams();

  return (
    <Container className={getPaneCssClassName("dirhcy", props.isVisible)}></Container>
  );
}