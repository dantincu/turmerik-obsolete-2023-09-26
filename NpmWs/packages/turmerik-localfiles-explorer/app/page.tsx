"use client"; // This is a client component 👈🏽

import { detect_mobile } from "@turmerik/next-react";
const useIsMobile = detect_mobile.useIsMobile;

export default function Home() {
  const isMobile = useIsMobile();

  return isMobile ? <div>Mobile variant</div> : <div>Desktop variant</div>;
}