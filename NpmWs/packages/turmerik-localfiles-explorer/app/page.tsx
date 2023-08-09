"use client"; // This is a client component 👈🏽

import TrmrkExplorer from '../components/TrmrkExplorer/TrmrkExplorer';
import { TrmrkExplorerProps, TrmrkExplorerPageName, defaultPageNameRetriever } from '../components/TrmrkExplorer/TrmrkExplorerProps';

export default function Home() {
  const props = {
    pageNameRetriever: defaultPageNameRetriever(0)
  } as TrmrkExplorerProps

  return <TrmrkExplorer {...props} />
}