import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:font_awesome_flutter/font_awesome_flutter.dart';
import 'package:inventory/pages/shared_widgets/custom_app_bar.dart';
import 'package:inventory/provider/tab_bar_item_provider.dart';

class HomePage extends StatefulWidget {
  const HomePage({
    Key? key,
    required this.user,
  }) : super(key: key);

  final User user;

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  int index = 1;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: CustomAppBar.buildStaticAppBar(context, widget.user),
      bottomNavigationBar: NavigationBarTheme(
        data: NavigationBarThemeData(
          iconTheme: MaterialStateProperty.all(Theme.of(context).iconTheme),
          indicatorColor: Colors.transparent,
          backgroundColor: Theme.of(context).primaryColor,
          labelTextStyle: MaterialStateProperty.all(
            TextStyle(
              fontSize: 14,
              fontWeight: FontWeight.w500,
              color: Theme.of(context).primaryColorLight,
            ),
          ),
        ),
        child: NavigationBar(
          height: 68,
          selectedIndex: index,
          labelBehavior: NavigationDestinationLabelBehavior.onlyShowSelected,
          animationDuration: const Duration(milliseconds: 1000),
          destinations: _buildTabItems(),
          onDestinationSelected: (index) => setState(() => this.index = index),
        ),
      ),
      body: TabBarItems.pages[index].widget,
    );
  }

  List<Widget> _buildTabItems() {
    return TabBarItems.pages
        .map((p) => NavigationDestination(
              icon: FaIcon(p.icon),
              label: p.label,
            ))
        .toList();
  }
}
