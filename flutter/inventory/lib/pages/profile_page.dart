import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:font_awesome_flutter/font_awesome_flutter.dart';
import 'package:inventory/pages/shared_widgets/buttons.dart';
import 'package:inventory/themes.dart';

class PersonalizationPage extends StatelessWidget {
  const PersonalizationPage({
    Key? key,
    required this.user,
  }) : super(key: key);

  final User user;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Theme.of(context).primaryColor,
        title: const Text('Profile'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(10.0),
        child: Center(
          child: Column(
            children: [
              const SizedBox(height: 10),
              _buildProfilePhoto(),
              const SizedBox(height: 10),
              _buildProfileName(),
              _buildProfileEmail(),
              const SizedBox(height: 28),
              const Text('App Theme'),
              _buildThemeSelectionGroup(context),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildProfilePhoto() {
    return Hero(
      tag: 'googleUserPhoto',
      child: CircleAvatar(
        backgroundImage: NetworkImage(user.photoURL!),
        radius: 38,
      ),
    );
  }

  Widget _buildProfileName() {
    return Text(
      user.displayName!,
      style: const TextStyle(
        fontSize: 28,
        fontWeight: FontWeight.bold,
        fontFamily: 'Popping',
        letterSpacing: 5,
      ),
    );
  }

  Widget _buildProfileEmail() {
    return Text(
      user.email!,
      style: const TextStyle(fontSize: 10, letterSpacing: 2),
    );
  }

  Widget _buildThemeSelectionGroup(BuildContext context) {
    final width = MediaQuery.of(context).size.width;
    return Card(
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(13),
      ),
      color: Theme.of(context).cardColor,
      child: Container(
        width: width,
        padding: const EdgeInsets.all(10.0),
        child: Consumer(builder: (context, ref, _) {
          final themeProvider = ref.watch(themeModeProvider);
          return Row(
            children: [
              Expanded(
                child: IconTextRadio<ThemeModeSelections>(
                  value: ThemeModeSelections.system,
                  selectedValue: themeProvider.selected,
                  label: 'System',
                  icon: FontAwesomeIcons.microchip,
                  onTap: () =>
                      themeProvider.updateThemeMode(ThemeModeSelections.system),
                ),
              ),
              Expanded(
                child: IconTextRadio<ThemeModeSelections>(
                  value: ThemeModeSelections.light,
                  selectedValue: themeProvider.selected,
                  label: 'Light',
                  icon: FontAwesomeIcons.solidSun,
                  onTap: () =>
                      themeProvider.updateThemeMode(ThemeModeSelections.light),
                ),
              ),
              Expanded(
                child: IconTextRadio<ThemeModeSelections>(
                  value: ThemeModeSelections.dark,
                  selectedValue: themeProvider.selected,
                  label: 'Dark',
                  icon: FontAwesomeIcons.solidMoon,
                  onTap: () =>
                      themeProvider.updateThemeMode(ThemeModeSelections.dark),
                ),
              ),
            ],
          );
        }),
      ),
    );
  }
}
