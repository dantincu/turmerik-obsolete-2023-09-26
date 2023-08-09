"use client"; // This is a client component 👈🏽

import { detect_mobile } from "@turmerik/next-react";
import { Container } from '@mui/material';
import { useSearchParams } from "next/navigation"
import "./TopBar.scss"

import { NewTabPaneProps } from './TopBarProps';

export default function TopBar(
  props: NewTabPaneProps
) {
  const isMobile = detect_mobile.useIsMobile()
  const urlQuery = useSearchParams();

  return (
    <div className={["trmrk-top-bar"].join(" ")}>

    </div>
  );
}