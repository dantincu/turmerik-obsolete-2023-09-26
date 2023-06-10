import { useState } from 'react';

import { Routes, Route, Outlet } from "react-router-dom";

import HomePage from "./pages/HomePage";
import ExplorerPage from "./pages/ExplorerPage";
import SettingsPage from "./pages/SettingsPage";

import AppBarComponent from './components/AppBarComponent';
import AppLoadingPage from './pages/AppLoadingPage';
import AppLoadingErrorPage from './pages/AppLoadingErrorPage';

import { AppSettingsService } from './services/AppSettings/AppSettingsService';

const appSettingsService = AppSettingsService.instance;
appSettingsService.init();

const App = () => {
  const appSettingsResponse = appSettingsService.appSettingsResponse;

  return (appSettingsResponse ? (
    appSettingsResponse.status.isSuccessfull ? (
      <div className="trmrk-app">
        <AppBarComponent></AppBarComponent>
        <Routes>
          <Route element={<Outlet />}>
            <Route path="/" element={<HomePage />} />
            <Route path="/settings" element={<SettingsPage />} />
            <Route path="/explorer/:path" element={<ExplorerPage />} />
          </Route>
        </Routes>
      </div>
    ) : (
      <div className="trmrk-app-error">
        <AppLoadingErrorPage />
      </div>
    )
  ) : (
    <div className="trmrk-app-loading">
      <AppLoadingPage />
    </div>
  ));
}

export default App;