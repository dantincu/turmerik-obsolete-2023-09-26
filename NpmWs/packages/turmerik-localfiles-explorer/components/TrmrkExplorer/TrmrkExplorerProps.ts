export interface TrmrkExplorerProps {
  pageNameRetriever: (pathSegments: string[]) => TrmrkExplorerPageName
}

export enum TrmrkExplorerPageName {
  Home = 0,
  NewTab,
  UserPreferences,
  DirHcy,
  Folder,
  FileView,
  TextFilePreview,
  TextFileEdit,
}

export const defaultPageNameRetriever = (pageNameSegmentIdx: number) => (pathSegments: string[]) => {
  let pageName = TrmrkExplorerPageName.Home

  switch (pathSegments[pageNameSegmentIdx]) {
    case "new-tab":
      pageName = TrmrkExplorerPageName.NewTab
      break
    case "user-preferences":
      pageName = TrmrkExplorerPageName.UserPreferences
      break
    case "dirhcy":
      pageName = TrmrkExplorerPageName.DirHcy
      break
    case "folder":
      pageName = TrmrkExplorerPageName.Folder
      break
    case "file-view":
      pageName = TrmrkExplorerPageName.FileView
      break
    case "text-file-preview":
      pageName = TrmrkExplorerPageName.TextFilePreview
      break
    case "text-file-edit":
      pageName = TrmrkExplorerPageName.TextFileEdit
      break
  }

  return pageName
}
