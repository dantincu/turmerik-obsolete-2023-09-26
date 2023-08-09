"use client"; // This is a client component 👈🏽

import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';

import type { Metadata } from 'next'
import { Inter } from 'next/font/google'

import './globals.css'
import { getInstance as getAppSettingsService } from './services/app-settings/app-settings-service'

const inter = Inter({ subsets: ['latin'] })

export const metadata: Metadata = {
  title: 'Turmerik Local Files Explorer',
  description: 'An alternative to the native file explorer',
}

import { services, components } from "@turmerik/next-react"
import { getInstance as getApiService } from "./services/api/api-service"

services.api.api_service.registerInstance(getApiService());
services.api.app_settings_service.registerInstance(getAppSettingsService());

export default function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <components.Layout props={{
      bodyClassName: [inter.className].join(" ")
    }}>{children}</components.Layout>
  );
}
