import 'package:flutter/material.dart';
import 'package:font_awesome_flutter/font_awesome_flutter.dart';
import 'package:inventory/models/tab_item.dart';
import 'package:inventory/pages/inventory_page.dart';
import 'package:inventory/pages/report_page.dart';
import 'package:inventory/pages/sale_page.dart';

class TabBarItems {
  static final List<TabItem> pages = [
    TabItem(
      index: 0,
      label: 'Sale',
      icon: FontAwesomeIcons.fileInvoiceDollar,
      backgroundColor: Colors.blue,
      widget: const SalePage(),
    ),
    TabItem(
      index: 1,
      label: 'Inventory',
      icon: FontAwesomeIcons.warehouse,
      widget: const InventoryPage(),
    ),
    TabItem(
      index: 2,
      label: 'Report',
      icon: FontAwesomeIcons.chartPie,
      widget: const ReportPage(),
    ),
  ];
}
