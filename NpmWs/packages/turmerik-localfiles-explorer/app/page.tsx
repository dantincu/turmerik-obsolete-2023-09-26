"use client"; // This is a client component 👈🏽

import { useIsMobile } from "./utils/useIsMobile";

export default function Home() {
  const isMobile = useIsMobile();

  return isMobile ? <div>Mobile variant</div> : <div>Desktop variant</div>;
}