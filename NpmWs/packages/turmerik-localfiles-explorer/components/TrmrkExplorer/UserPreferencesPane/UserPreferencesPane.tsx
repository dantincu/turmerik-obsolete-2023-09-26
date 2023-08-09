"use client"; // This is a client component 👈🏽

import { detect_mobile } from "@turmerik/next-react";
import { Container } from '@mui/material';
import { useSearchParams } from "next/navigation"
import "./UserPreferencesPane.scss"

import { UserPreferencesPaneProps } from './UserPreferencesPaneProps';
import { getPaneCssClassName } from '../classNames';

export default function UserPreferencesPane(
  props: UserPreferencesPaneProps
) {
  const isMobile = detect_mobile.useIsMobile()
  const urlQuery = useSearchParams();

  return (
    <Container className={getPaneCssClassName("user-preferences", props.isVisible)}></Container>
  );
}