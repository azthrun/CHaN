import 'package:flutter/material.dart';

class TabItem {
  int index;
  String label;
  IconData icon;
  Widget widget;
  Color? backgroundColor;

  TabItem({
    required this.index,
    required this.label,
    required this.icon,
    required this.widget,
    this.backgroundColor,
  });
}
