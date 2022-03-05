import 'package:firebase_auth/firebase_auth.dart';
import 'package:firebase_core/firebase_core.dart';
import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:inventory/auth_handler.dart';
import 'package:inventory/pages/error_page.dart';
import 'package:inventory/pages/loading_page.dart';
import 'package:inventory/provider/authentication_provider.dart';
import 'package:inventory/themes.dart';

Future main() async {
  WidgetsFlutterBinding.ensureInitialized();
  runApp(const ProviderScope(child: MyApp()));
}

final firebaseInitProvider = FutureProvider<FirebaseApp>((ref) async {
  return await Firebase.initializeApp();
});

final authenticationProvider =
    Provider<AuthenticationProvider>((ref) => AuthenticationProvider());

final authStateProvider = StreamProvider<User?>(
    (ref) => ref.read(authenticationProvider).authStateChange);

class MyApp extends StatelessWidget {
  const MyApp({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Consumer(
      builder: (context, ref, _) {
        final themeProvider = ref.watch(themeModeProvider);
        return MaterialApp(
          title: 'Inventory',
          themeMode: themeProvider.themeMode,
          theme: AppThemes.lightTheme,
          darkTheme: AppThemes.darkTheme,
          home: Consumer(
            builder: (context, ref, _) {
              final firebaseInit = ref.watch(firebaseInitProvider);
              return firebaseInit.when(
                data: (_) => const AuthHandler(),
                error: (e, stack) => ErrorPage(exception: e, stackTrace: stack),
                loading: () => const LoadingPage(),
              );
            },
          ),
          debugShowCheckedModeBanner: false,
        );
      },
    );
  }
}
