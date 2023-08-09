"use client"; // This is a client component 👈🏽

import React, { Suspense } from "react";
import { detect_mobile } from "@turmerik/next-react";
import { Container } from '@mui/material';
import { useSearchParams, usePathname, redirect } from "next/navigation"
import "./TrmrkExplorer.scss"

import { TrmrkExplorerProps, TrmrkExplorerPageName } from './TrmrkExplorerProps';

const TopBar = React.lazy(() => import('./TopBar/TopBar'))
const NewTabPane = React.lazy(() => import('./NewTabPane/NewTabPane'))
const HomePane = React.lazy(() => import('./HomePane/HomePane'))
const UserPreferencesPane = React.lazy(() => import('./UserPreferencesPane/UserPreferencesPane'))
const DirHcyPane = React.lazy(() => import('./DirHcyPane/DirHcyPane'))
const FolderPane = React.lazy(() => import('./FolderPane/FolderPane'))
const TextEditorPane = React.lazy(() => import('./TextEditorPane/TextEditorPane'))
const PreviewPane = React.lazy(() => import('./PreviewPane/PreviewPane'))
const ReadonlyPane = React.lazy(() => import('./ReadonlyPane/ReadonlyPane'))

export default function TrmrkExplorer(
  props: TrmrkExplorerProps
) {
  const isMobile = detect_mobile.useIsMobile()
  const urlQuery = useSearchParams();
  const pathSegments = (usePathname() ?? "").split("/")

  const pageName = props.pageNameRetriever(pathSegments);
  const itemPath = urlQuery?.get("item-path") ?? ""
  const tabIdx = urlQuery?.get("tab-idx") ?? 0
  const isDesktop = !isMobile;

  let newTabPaneVisible = pageName === TrmrkExplorerPageName.NewTab
  let homePaneVisible = pageName === TrmrkExplorerPageName.Home
  let userPreferencesPaneVisible = pageName === TrmrkExplorerPageName.UserPreferences
  let dirHcyPaneVisible = pageName === TrmrkExplorerPageName.DirHcy || isDesktop
  let folderPaneVisible = pageName === TrmrkExplorerPageName.Folder
  let textEditorPaneVisible = pageName === TrmrkExplorerPageName.TextFileEdit
  let previewPaneVisible = pageName === TrmrkExplorerPageName.TextFilePreview
  let readonlyPaneVisible = pageName === TrmrkExplorerPageName.FileView

  if (pageName === TrmrkExplorerPageName.NewTab) {

  } else if (pageName === TrmrkExplorerPageName.Home) {

  } else if (pageName === TrmrkExplorerPageName.UserPreferences) {
    
  }

  return (
    <main className="trmrk-explorer h-full w-full grid grid-cols-3">
      { isMobile ? (<div className="trmrk-header">
      </div>) : (<div className="trmrk-header">
        <TopBar></TopBar>
      </div>) }
      <div className="trmrk-main-panel">
      </div>
    </main>
  );
}