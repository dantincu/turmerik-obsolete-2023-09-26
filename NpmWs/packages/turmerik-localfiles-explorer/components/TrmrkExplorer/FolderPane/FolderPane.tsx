"use client"; // This is a client component 👈🏽

import { detect_mobile } from "@turmerik/next-react";
import { Container } from '@mui/material';
import { useSearchParams } from "next/navigation"
import "./FolderPane.scss"

import { FolderPaneProps } from './FolderPaneProps';
import { getPaneCssClassName } from '../classNames';

export default function FolderPane(
  props: FolderPaneProps
) {
  const isMobile = detect_mobile.useIsMobile()
  const urlQuery = useSearchParams();

  return (
    <Container className={getPaneCssClassName("folder", props.isVisible)}></Container>
  );
}