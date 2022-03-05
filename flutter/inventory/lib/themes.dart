import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

final themeModeProvider =
    ChangeNotifierProvider<ThemeProvider>((ref) => ThemeProvider());

enum ThemeModeSelections { system, light, dark }

class ThemeProvider extends ChangeNotifier {
  ThemeMode themeMode = ThemeMode.system;
  ThemeModeSelections selected = ThemeModeSelections.system;

  void updateThemeMode(ThemeModeSelections? selection) {
    selected = selection!;
    if (selected == ThemeModeSelections.system) {
      themeMode = ThemeMode.system;
    } else if (selected == ThemeModeSelections.light) {
      themeMode = ThemeMode.light;
    } else if (selected == ThemeModeSelections.dark) {
      themeMode = ThemeMode.dark;
    } else {
      throw '$selection mode is not defined';
    }
    notifyListeners();
  }
}

class AppThemes {
  static final lightTheme = ThemeData(
    scaffoldBackgroundColor: Colors.grey[300],
    colorScheme: const ColorScheme.light(),
    iconTheme: IconThemeData(color: Colors.grey[200], opacity: 0.8),
    floatingActionButtonTheme: FloatingActionButtonThemeData(
      backgroundColor: Colors.deepOrange[400],
      foregroundColor: Colors.grey[100],
    ),
    primaryColor: Colors.deepPurpleAccent,
    primaryColorLight: Colors.grey[100],
    primaryColorDark: Colors.blue[400],
    indicatorColor: Colors.deepPurpleAccent[100],
    cardColor: Colors.grey[400],
    highlightColor: Colors.blue[300],
  );

  static final darkTheme = ThemeData(
    scaffoldBackgroundColor: Colors.grey[800],
    colorScheme: const ColorScheme.dark(),
    iconTheme: IconThemeData(color: Colors.grey[300], opacity: 0.8),
    floatingActionButtonTheme: FloatingActionButtonThemeData(
      backgroundColor: Colors.deepOrange[600],
      foregroundColor: Colors.grey[200],
    ),
    primaryColor: Colors.deepPurple,
    primaryColorLight: Colors.grey[200],
    primaryColorDark: Colors.blue[700],
    indicatorColor: Colors.deepPurpleAccent[100],
    cardColor: Colors.grey,
    highlightColor: Colors.blue[700],
  );
}
