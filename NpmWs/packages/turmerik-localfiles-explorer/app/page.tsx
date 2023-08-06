"use client"; // This is a client component 👈🏽

import { detect_mobile } from "@turmerik/next-react";

export default function Home() {
  const isMobile = detect_mobile.useIsMobile()

  return isMobile ? <div>Mobile Version</div> : <div>Desktop Version</div>;
}