import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:inventory/main.dart';
import 'package:inventory/pages/shared_widgets/buttons.dart';
import 'package:inventory/provider/authentication_provider.dart';

enum LoadingState { init, loading }

class LoginPage extends StatefulWidget {
  const LoginPage({Key? key}) : super(key: key);

  @override
  State<LoginPage> createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  LoadingState _state = LoadingState.init;
  bool _isAnimating = true;

  Future _signInWithGoogle(AuthenticationProvider auth) async {
    setState(() => _state = LoadingState.loading);
    await auth
        .signInWithGoogle(context)
        .whenComplete(() => auth.authStateChange.listen((event) {
              if (event == null) {
                setState(() => _state = LoadingState.init);
                return;
              }
            }));
  }

  @override
  Widget build(BuildContext context) {
    final width = MediaQuery.of(context).size.width;
    final isStretched = _isAnimating || _state == LoadingState.init;
    return Scaffold(
      body: SafeArea(
        child: Container(
          width: width,
          decoration: const BoxDecoration(
            image: DecorationImage(
              image: AssetImage('assets/defaultbg.jpg'),
              fit: BoxFit.cover,
            ),
          ),
          child: Column(
            children: [
              const SizedBox(height: 60.0),
              const SizedBox.square(
                dimension: 130.0,
                child: Image(
                  image: AssetImage('assets/appicon.png'),
                ),
              ),
              const SizedBox(height: 20),
              Text(
                'Inventory',
                style: TextStyle(
                  color: Theme.of(context).primaryColorLight,
                  letterSpacing: 5.0,
                  fontSize: 28.0,
                  fontWeight: FontWeight.bold,
                  fontFamily: 'SOne',
                ),
              ),
              const Spacer(),
              AnimatedContainer(
                duration: const Duration(milliseconds: 300),
                curve: Curves.easeIn,
                onEnd: () => setState(() => _isAnimating = !_isAnimating),
                width: _state == LoadingState.init ? width * 0.8 : 58,
                height: 58,
                child: isStretched
                    ? Consumer(
                        builder: (context, ref, _) {
                          final auth = ref.watch(authenticationProvider);
                          return LoginButton(buttonPressed: () async {
                            await _signInWithGoogle(auth);
                          });
                        },
                      )
                    : const LoginButtonAlt(),
              ),
              const SizedBox(height: 30.0),
            ],
          ),
        ),
      ),
    );
  }
}
